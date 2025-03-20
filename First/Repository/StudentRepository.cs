using First.Data;
using First.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace First.Repository
{
    public class StudentRepository<T> : IStudent<T> where T : class
    {
        private readonly HttpClient _client;
        private readonly Uri _baseAddress;

        public StudentRepository(HttpClient client)
        {
            _baseAddress = new Uri("https://localhost:7026");
            _client = client;
            _client.BaseAddress = _baseAddress;
        }
        
        //API
        // Fetch all students
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/stud/get");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var students = JsonConvert.DeserializeObject<List<Student>>(data);
                return students;
            }
            return null; // or throw an exception based on your logic
        }

        public async Task<T> CreateAsync(T entity)
        {
            var json = JsonConvert.SerializeObject(entity);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("/api/stud/post", data);
            if (response.IsSuccessStatusCode)
            {
                return entity;
            }
            return null; // or throw an exception based on your logic
        }

        public async Task<T> GetByIDAsync(int id)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/stud/get/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var student = JsonConvert.DeserializeObject<Student>(data);
                return student as T;
            }
            return null; // or throw an exception based on your logic
        }

        public async Task<T> UpdateAsync(Student s)
        {
            var json = JsonConvert.SerializeObject(s);//Here the object student is serialized to json
            var data = new StringContent(json, Encoding.UTF8, "application/json");//Here the json is converted to string content to post in api
            HttpResponseMessage response = await _client.PutAsync("/api/stud/put", data);//Here the data is posted to api
            if (response.IsSuccessStatusCode)
            {
                return s as T;
            }

            return null;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"/api/stud/delete/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
/*
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;
        public StudentRepository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }
        //Create Data..
        public async Task<T> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _db.SaveChangesAsync();
                return entity; 
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("Could not save data to db.", ex);
            }
        }
        //Delete Data..
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return false;
                }
                _dbSet.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new ApplicationException("Could not delete data from db.", ex);
            }
        }

        //Get All Data..
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not fench data from db.", ex);
            }
        }
        //Filter by ID..
        public async Task<T> GetByIDAsync(int id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
                throw new ApplicationException("Could not find data in db.");
            }
            catch (Exception ex)
            {
                throw new Exception("Could not fench data from db.", ex);
            }
        }
        //Update Data..
        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                await _db.SaveChangesAsync();
                return entity;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ApplicationException("Could not update data in db.", ex);
            }
        }*/
