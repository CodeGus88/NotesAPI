using AutoMapper;
using NotesAPI.DTOs;
using NotesAPI.Models;

namespace NotesAPI.Utils
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<NoteRequest, NoteEntity>();
        }
    }
}
