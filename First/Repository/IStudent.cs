using First.Models;

namespace First.Repository
{
    public interface IStudent<T> where T : class
    {
        //Get All Data..
        //Task<IEnumerable<T>> GetAllAsync();
        //Getting data using API
        Task<List<Student>> GetAllStudentsAsync();
        Task<T> CreateAsync(T entity);//This function will create a new student.

        Task<T> GetByIDAsync(int id);//This function will update the student details.

        Task<T> UpdateAsync(Student s);//This function will update the student details.
        Task<bool> DeleteAsync(int id);//This function will delete the student details.
        //Filter by ID..
        /*Task<T>GetByIDAsync(int id);
        //Create Data..
        
        //Update Data..
        Task<T> UpdateAsync(T entity);
        //Delete Data..
        Task<bool> DeleteAsync(int id);
        */
    }
}