using AutoMapper;
using SimpleTasks.Data.Models;
using SimpleTasks.Services.Models;
using System;

namespace SimpleTasks.Services.MappingProfiles
{
    public class TaskProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Task, TaskModel>()
                .ForMember(dest => dest.DueDate, x => x.MapFrom(src => new DateTime(src.DueDate.Ticks, DateTimeKind.Utc)))
                .ForMember(dest => dest.CompletedDateTime, x => x.MapFrom(src => MapNullableDateTime(src.CompletedDateTime)))
                .ForMember(dest => dest.IsComplete, x => x.MapFrom(src => src.IsComplete()));

            Mapper.CreateMap<TaskModel, Task>()
                .ForMember(dest => dest.DueDate, x => x.MapFrom(src => new DateTime(src.DueDate.Ticks, DateTimeKind.Utc)))
                .ForMember(dest => dest.CompletedDateTime, x => x.MapFrom(src => MapNullableDateTime(src.CompletedDateTime)));
        }

        public static DateTime? MapNullableDateTime(DateTime? src)
        {
            if (src.HasValue)
            {
                return new DateTime(src.Value.Ticks, DateTimeKind.Utc);
            }
            
            return null;
        }
    }
}
