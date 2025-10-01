﻿using System.ComponentModel.DataAnnotations;

namespace DirectoryService.Models.DTOs
{
    /// <summary>
    /// DTO для ответа с информацией об активных окнах подразделения
    /// </summary>
    public class DepartmentWorkplacesResponseDto
    {
        /// <summary>
        /// Список рабочих мест подразделения
        /// </summary>
        [Display(Name = "Список окон подразделения")]
        public List<WorkplaceDto> Workplaces { get; }

        public DepartmentWorkplacesResponseDto(List<WorkplaceDto> workplaces)
        {
            Workplaces = workplaces;
        }

        /// <summary>
        /// DTO с информацией об окне
        /// </summary>
        public class WorkplaceDto
        {
            /// <summary>
            /// Уникальный идентификатор окна
            /// </summary>
            [Display(Name = "Уникальный идентификатор окна")]
            public Guid Id { get; }

            /// <summary>
            /// Уникальный код окна
            /// </summary>
            [Display(Name = "Уникальный код окна")]
            public string Code { get; }

            /// <summary>
            /// Название окна
            /// </summary>
            [Display(Name = "Название окна")]
            public string Name { get; }

            /// <summary>
            /// Список услуг, оказываемых окном
            /// </summary>
            [Display(Name = "Услуги, оказываемые окном")]
            public List<ServiceInfoDto> Services { get; }

            public WorkplaceDto(Guid id, string code, string name, List<ServiceInfoDto> services)
            {
                Id = id;
                Code = code;
                Name = name;
                Services = services;
            }
        }

        /// <summary>
        /// DTO с информацией об услугах
        /// </summary>
        public class ServiceInfoDto
        {
            [Display(Name = "ID категории услуги")]
            public Guid CategoryId { get; }

            [Display(Name = "Код категории услуги")]
            public string CategoryCode { get; }

            [Display(Name = "Название категории услуги")]
            public string CategoryName { get; }

            [Display(Name = "ID услуги")]
            public Guid ServiceId { get; }

            [Display(Name = "Код услуги")]
            public string ServiceCode { get; }

            [Display(Name = "Название услуги")]
            public string ServiceName { get; }

            public ServiceInfoDto(
                Guid categoryId,
                string categoryCode,
                string categoryName,
                Guid serviceId,
                string serviceCode,
                string serviceName)
            {
                CategoryId = categoryId;
                CategoryCode = categoryCode;
                CategoryName = categoryName;
                ServiceId = serviceId;
                ServiceCode = serviceCode;
                ServiceName = serviceName;
            }
        }
    }
}