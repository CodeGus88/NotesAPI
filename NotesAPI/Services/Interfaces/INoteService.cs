﻿using NotesAPI.DTOs;
using NotesAPI.Entities;

namespace NotesAPI.Services.Interfaces
{
    public interface INoteService
    {
        Task<Note> Add(NoteRequest request);
        Task<Note> FindById(Guid id);
        Task<List<Note>> GetAll();
        Task Delete(Guid id);
        Task Edit(Guid id, NoteRequest request);
        Task<bool> ExistsById(Guid id);
    }
}