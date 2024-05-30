using TutorDemand.Business;
using TutorDemand.Business.Abstractions;
using TutorDemand.Data.Entities;

namespace TutorDemand.RazorWebApp.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureBusinesses(this IServiceCollection services)
        {
            services.AddScoped<ITutorBusiness, TutorBusiness>();
            services.AddScoped<ITeachingScheduleBusiness, TeachingScheduleBusiness>();
            services.AddScoped<ICustomerBusiness, CustomerBusiness>();
            services.AddScoped<IImageBusiness, ImageBusiness>();
            services.AddScoped<IReservationBusiness, ReservationBusiness>();
        }

        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration) =>
            services.AddSqlServer<NET1704_221_5_TutorDemandContext>(
                configuration.GetConnectionString("TutorDemandContextConnection"));

        public static void RegisterMapsterConfiguration(this IServiceCollection _)
        {
            //TypeAdapterConfig<Hub, HubWithLastMessageDto>
            //    .NewConfig()
            //    .Map(dest => dest.LastMessage,
            //        src => src.Messages.Count != 0
            //            ? src.Messages.ToList()[0].Adapt<MessageWithSenderDto>()
            //            : null);

            //TypeAdapterConfig<Hub, HubDetailDto>
            //    .NewConfig()
            //    .Map(dest => dest.OtherUser,
            //        src => src.Users.Count != 0
            //            ? src.Users.ToList()[0].Adapt<UserWithProfileDto>()
            //            : null);
        }
    }
}