using AutoMapper;
using NotesAPI.DTOs;
using NotesAPI.EnumsAndStatics;
using NotesAPI.Models;
using NotesAPI.Repositories.Interfaces;
using NotesAPI.Services.Interfaces;

namespace NotesAPI.Services
{
    public class NoteService : INoteService
    {
        protected readonly INoteRepository repository;
        private readonly IRedisRepository cacheRepository;
        private readonly IMapper mapper;

        public NoteService(INoteRepository repository, IRedisRepository cacheRepository, IMapper mapper)
        {
            this.repository = repository;
            this.cacheRepository = cacheRepository;
            this.mapper = mapper;
        }

        #region cacheitem
        public async Task<NoteEntity> Add(NoteRequest request)
        {
            NoteEntity note = mapper.Map<NoteEntity>(request);
            note.Id = Guid.NewGuid();
            await repository.InsertAsync(note);
            await cacheRepository.Set(GetItemKey(note.Id), note);
            return note;
        }
        public async Task Delete(Guid id)
        {
            await repository.DeleteAsync(new NoteEntity { Id = id});
            await cacheRepository.Delete(GetItemKey(id));
        }
        public async Task Edit(Guid id, NoteRequest request)
        {
            NoteEntity note = mapper.Map<NoteEntity>(request);
            note.Id = id;
            await repository.UpdateAsync(note);
            await cacheRepository.Set(GetItemKey(note.Id), note);
        }
        public async Task<NoteEntity> FindById(Guid id, bool useCache)
        {
            NoteEntity note;
            if (useCache)
            {
                bool existInCache = await cacheRepository.ExistsKey(GetItemKey(id));
                if (existInCache)
                {
                    note = await cacheRepository.FindByKey<NoteEntity>(GetItemKey(id));
                }
                else {
                    note = await repository.GetAsync(new NoteEntity { Id = id });
                    await cacheRepository.Set(GetItemKey(id), note);
                }
            }
            else 
            {
                note = await repository.GetAsync(new NoteEntity { Id = id });
            }
            return note;
        }

        public async Task<List<NoteEntity>> GetAll(bool useCache)
        {
            List<NoteEntity> notes = await cacheRepository.GetAll<NoteEntity>(PartialKey.NOTE);
            if (useCache)
            {
                if (notes.Count() == 0)
                {
                    notes = (await repository.GetAllAsync()).ToList();
                    notes.ForEach(async item =>
                        await cacheRepository.Set($"{PartialKey.NOTE}{item.Id}", item)
                    );
                }
            }
            else {
                notes = (await repository.GetAllAsync()).ToList();
                notes.ForEach(async note => {
                    bool exists = await cacheRepository.ExistsKey(GetItemKey(note.Id));
                    if (!exists) await cacheRepository.Set(GetItemKey(note.Id), note);
                });
            }
            notes = (await repository.GetAllAsync()).ToList();

            return notes;
        }

        public async Task<bool> ExistsById(Guid id, bool useCache) {
            if (useCache)
            {
                var exists = await cacheRepository.ExistsKey(GetItemKey(id));
                if (!exists) return await repository.ExistsByIdAsync(ETable.Notes, id);
                return exists;
            }
            return await repository.ExistsByIdAsync(ETable.Notes, id);
        }

        private string GetItemKey(Guid id)
        {
            return $"{PartialKey.NOTE}{id}";
        }
        #endregion


        #region cachelist
        public async Task<NoteEntity> Add2(NoteRequest request)
        {
            NoteEntity note = mapper.Map<NoteEntity>(request);
            note.Id = Guid.NewGuid();
            await repository.InsertAsync(note);

            List<NoteEntity> notes = (await cacheRepository.FindByKey<List<NoteEntity>>(PartialKey.NOTES)) ?? new List<NoteEntity>();
            notes.Add(note);
            await cacheRepository.Set(PartialKey.NOTES, notes);

            return note;
        }

        public async Task<NoteEntity> FindById2(Guid id, bool useCache )
        {
            
            NoteEntity note;
            if (useCache)
            {
                List<NoteEntity> notes = (await cacheRepository.FindByKey<List<NoteEntity>>(PartialKey.NOTES)) ?? new List<NoteEntity>();
                if (notes.Count == 0)
                {
                    note = notes.FirstOrDefault(item => item.Id == id);
                    if (note == null) {
                        note = await repository.GetAsync(new NoteEntity { Id = id });
                        notes.Add(note);
                        await cacheRepository.Set(PartialKey.NOTES, notes);
                    }
                }
                else
                {
                    note = await repository.GetAsync(new NoteEntity { Id = id});
                    await cacheRepository.Set(PartialKey.NOTES, new List<NoteEntity> { note });
                }
            }
            else
            {
                note = await repository.GetAsync(new NoteEntity { Id = id } );
            }
            return note;
        }

        public async Task<List<NoteEntity>> GetAll2(bool useCache)
        {
            List<NoteEntity> notes;
            if (useCache)
            {
                notes = (await cacheRepository.FindByKey<List<NoteEntity>>(PartialKey.NOTES)) ?? new List<NoteEntity>();
                if (notes.Count == 0) {
                    notes = (await repository.GetAllAsync()).ToList();
                    if (notes.Count > 0) 
                        await cacheRepository.Set(PartialKey.NOTES, notes);
                }
            }
            else {
                notes = (await repository.GetAllAsync()).ToList();
                await cacheRepository.Set(PartialKey.NOTES, notes);
            }
            return notes;
        }

        public async Task Delete2(Guid id)
        {
            await repository.DeleteAsync(new NoteEntity { Id = id });

            List<NoteEntity> notes = (await cacheRepository.FindByKey<List<NoteEntity>>(PartialKey.NOTES)) ?? new List<NoteEntity>();
            notes.RemoveAll(note => note.Id == id);
            await cacheRepository.Set(PartialKey.NOTES, notes);
        }

        public async Task Edit2(Guid id, NoteRequest request)
        {
            NoteEntity note = mapper.Map<NoteEntity>(request);
            note.Id = id;
            await repository.UpdateAsync(note);

            List<NoteEntity> notes = (await cacheRepository.FindByKey<List<NoteEntity>>(PartialKey.NOTES)) ?? new List<NoteEntity>();
            if (notes.Exists(i => i.Id == id)) {
                var prepareNote = notes.Find(i => i.Id == id);
                prepareNote.Title = note.Title;
                prepareNote.Content = note.Content;
            }   
            else
                notes.Add(note);
            await cacheRepository.Set(PartialKey.NOTES, notes);
        }

        public async Task<bool> ExistsById2(Guid id, bool useCache)
        {
            if (useCache)
            {
                List<NoteEntity> notes = (await cacheRepository.FindByKey<List<NoteEntity>>(PartialKey.NOTES)) ?? new List<NoteEntity>();
                bool exists = notes.Exists(i => i.Id == id);
                if (!exists) return  await repository.ExistsByIdAsync(ETable.Notes, id);
                return exists;
            }
            else {
                return await repository.ExistsByIdAsync(ETable.Notes, id);
            }
        }
        #endregion
    }
}
