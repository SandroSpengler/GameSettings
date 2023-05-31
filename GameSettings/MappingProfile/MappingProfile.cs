using GameSettings.MVVM.Models;
using GameSettings.MVVM.ViewModels;

namespace GameSettings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ReleaseNote, ReleaseNoteListViewModel>().ReverseMap();
        }
    }
}

