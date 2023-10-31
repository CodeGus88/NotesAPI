using AutoMapper;
using NotesAPI.DTOs;
using NotesAPI.Entities;

namespace NotesAPI.Utils
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NoteRequest, Note>();
        }
    }
}
