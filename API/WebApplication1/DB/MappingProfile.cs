using AutoMapper;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.DB
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //CreateMap<User, UserDto>().ReverseMap();
            //CreateMap<Account, AccountDto>().ReverseMap();
            //CreateMap<Transaction, TransactionDto>().ReverseMap();
            //CreateMap<UserRegistrationDto, User>();
            //CreateMap<ChatMessage, ChatMessageDto>().ReverseMap();
            CreateMap<User, UserDto>()
            .ForMember(dest => dest.Accounts, opt => opt.MapFrom(src => src.Accounts))
            .ReverseMap()
            .ForMember(dest => dest.Accounts, opt => opt.Ignore());
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<UserKey, UserKeyDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>()
    .           ForMember(dest => dest.PaymentChannel, opt => opt.MapFrom(src => src.PaymentChannel.ToString()))
    .           ReverseMap()
    .           ForMember(dest => dest.PaymentChannel, opt => opt.MapFrom(src => Enum.Parse<PaymentChannel>(src.PaymentChannel)));
            CreateMap<UserRegistrationDto, User>();
            CreateMap<ChatMessage, ChatMessageDto>().ReverseMap();

        }
    }
}
