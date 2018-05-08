// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Linq;
using Project.Domain.Events;
using Project.Domain.SeedWork;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Project.Domain.AggregatesModel
{
    public class Project : Entity, IAggregateRoot
    {
        public int UserId { get; set; }
        public string Avator { get; set; }
        public string Company { get; set; }
        public string OriginBPFile { get; set; }
        public string ForamteBPFile { get; set; }
        public bool ShowSecurityInfo { get; set; }
        public int ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
        public DateTime RegisterDateTime { get; set; }
        public string Introduction { get; set; }
        /// <summary>
        /// 出让股份比例
        /// </summary>
        public string FinPercentag { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }
        /// <summary>
        /// 融资金额(w)
        /// </summary>
        public int FinMoney { get; set; }
        /// <summary>
        /// 收入(w)
        /// </summary>
        public int Income { get; set; }
        /// <summary>
        /// 利润(w)
        /// </summary>
        public int Revenue { get; set; }
        /// <summary>
        /// 估值(w)
        /// </summary>
        public int Valuation { get; set; }
        /// <summary>
        /// 佣金分配方式
        /// </summary>
        public int BrokerageOptions { get; set; }
        /// <summary>
        /// 是否委托平台
        /// </summary>
        public bool OnPlatform { get; set; }
        /// <summary>
        /// 可见范围
        /// </summary>
        public ProjectVisableRule ProjectVisableRule { get; set; }
        /// <summary>
        /// 根引用项目Id
        /// </summary>
        public int SourceId { get; set; }
        /// <summary>
        /// 上级引用项目Id
        /// </summary>
        public int RefenceId { get; set; }
        /// <summary>
        /// 项目标签
        /// </summary>
        public string Tags { get; set; }
        /// <summary>
        /// 项目属性集合
        /// </summary>
        public List<ProjectPropetry> ProjectPropetries { get; set; }
        /// <summary>
        /// 项目贡献者集合
        /// </summary>
        public List<ProjectContributor> ProjectContributors { get; set; }
        /// <summary>
        /// 项目查看着集合
        /// </summary>
        public List<ProjectViewer> ProjectViewers { get; set; }

        public DateTime UpdateTime { get; set; }
        public DateTime CreateTime { get; private set; }

        private Project ProjectClone(Project source)
        {
            if (source == null)
            {
                source = this;
            }
            Project project = new Project()
            {
                AreaId = source.AreaId,
                AreaName = source.AreaName,
                Avator = source.Avator,
                BrokerageOptions = source.BrokerageOptions,
                Company = source.Company,
                CityId = source.CityId,
                CityName = source.CityName,
                CreateTime = source.CreateTime,
                ProjectContributors = new List<ProjectContributor>(),
                FinMoney = source.FinMoney,
                FinPercentag = source.FinPercentag,
                FinStage = source.FinStage,
                ForamteBPFile = source.ForamteBPFile,
                OriginBPFile = source.OriginBPFile,
                Income = source.Income,
                Introduction = source.Introduction,
                OnPlatform = source.OnPlatform,
                ProjectPropetries = new List<ProjectPropetry>(),
                ProjectViewers = new List<ProjectViewer>(),
                ProjectVisableRule = source.ProjectVisableRule,
                ProvinceId = source.ProvinceId,
                ProvinceName = source.ProvinceName,
                RefenceId = source.RefenceId,
                RegisterDateTime = source.RegisterDateTime,
                Revenue = source.Revenue,
                ShowSecurityInfo = source.ShowSecurityInfo,
                SourceId = source.SourceId,
                Tags = source.Tags,
                UpdateTime = source.UpdateTime,
                UserId = source.UserId,
                Valuation = source.Valuation
            };
            if (source.ProjectContributors != null && source.ProjectContributors.Any())
            {
                project.ProjectContributors.AddRange(source.ProjectContributors);
            }
            if (source.ProjectPropetries != null && source.ProjectPropetries.Any())
            {
                project.ProjectPropetries.AddRange(source.ProjectPropetries);
            }
            if (source.ProjectViewers != null && source.ProjectViewers.Any())
            {
                project.ProjectViewers.AddRange(source.ProjectViewers);
            }
            return project;
        }

        public Project ContrbutorFork(int contrbutorId, Project source = null)
        {
            if (source == null) source = this;
            var newProject = ProjectClone(source);
            newProject.UserId = contrbutorId;
            newProject.SourceId = source.SourceId == 0 ? source.Id : source.SourceId;
            newProject.RefenceId = source.RefenceId == 0 ? source.Id : source.RefenceId;
            newProject.UpdateTime = DateTime.Now;
            return newProject;
        }

        public Project()
        {
            this.ProjectContributors = new List<ProjectContributor>();
            this.ProjectPropetries = new List<ProjectPropetry>();
            this.ProjectViewers = new List<ProjectViewer>();
            AddDomainEvent(new ProjectCreatedEvent() { Project = this });
        }

        public void AddViewer(string userName, string avator, int userid)
        {
            if (ProjectViewers.Any(x => x.UserId == userid)) return;
            var item = new ProjectViewer()
            {
                Avator = avator,
                CreateTime = DateTime.Now,
                ProjectId = Id,
                UserName = userName,
                UserId = userid
            };
            ProjectViewers.Add(item);
            AddDomainEvent(new ProejctViewedEvent() { ProjectViewer = item });
        }

        public void AddContributor(ProjectContributor contributor)
        {
            if (ProjectContributors.Any(x => x.UserId == contributor.UserId)) return;
            ProjectContributors.Add(contributor);
            AddDomainEvent(new ProjectJoinedEvent() { ProjectContributor = contributor });
        }
    }
}