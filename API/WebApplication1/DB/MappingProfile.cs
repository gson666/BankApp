using AutoMapper;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.DB
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Account, AccountDto>().ReverseMap();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<UserRegistrationDto, User>();
            CreateMap<ChatMessage, ChatMessageDto>().ReverseMap();

        }
    }
}
