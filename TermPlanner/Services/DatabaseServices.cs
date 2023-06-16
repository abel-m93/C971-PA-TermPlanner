using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TermPlanner.Models;
using Xamarin.Essentials;

namespace TermPlanner.Services
{
    public static class DatabaseServices
    {
        private static SQLiteAsyncConnection conn;


        //initializes connection to DB
        public static async Task Init()
        {
            if(conn != null)  //checks if DB already exists if NULL it would not exist and would create
            {                 //if NOT NULL it returns out of the INIT function
                return;
            }
            
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "TermPlanner.db");

            conn = new SQLiteAsyncConnection(dbPath);

            await conn.CreateTableAsync<Term>();
            await conn.CreateTableAsync<Course>();
            await conn.CreateTableAsync<Assessment>();
        }

        //Term CRUD Services

        #region TERM Database Services

        
        public static async Task AddTerm(string name, string status, DateTime start, DateTime end)
        {
            await Init();
            Term term = new Term()
            {
                Name = name,
                Status = status,
                StartDate = start,
                EndDate = end
            };

            await conn.InsertAsync(term);
            //AUTO INCREMENTED ID AUTOMATIICALY GENERATED
            int id = term.Id;
        }

        public static async Task DeleteTerm(int id)
        {
            await Init();
           // await conn.DeleteAsync(id);
            await conn.DeleteAsync<Term>(id);

        }

        public static async Task<IEnumerable<Term>> GetTerms()
        {
            await Init();
            //List<Term> terms = await conn.Table<Term>().ToListAsync();
            var terms = await conn.Table<Term>().ToListAsync();
            return terms;
        }

        public static async Task UpdateTerm(int id, string name, string status, DateTime start, DateTime end)
        {
            await Init();
            //creates Table interface and queries it with Term Id. If param finds a match with ID found in DB
            // return first result, should only be one result
            
            Term termQuery = await conn.Table<Term>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
            //if query returns a result (finds a match) update properties

            if (termQuery != null)
            {
                termQuery.Name = name;
                termQuery.Status = status;
                termQuery.StartDate = start;
                termQuery.EndDate = end;

                await conn.UpdateAsync(termQuery);
            }
            
        }



        #endregion
        //BEGIN COURSE CRUD Services


        #region COURSE Database Services

        
        public static async Task AddCourse(int termId, string name, string status, DateTime startDate, DateTime endDate,
            bool alertOn, string instrName, string instrPhone, string instrEmail, string notes)
        {
            await Init();
            Course course = new Course()
            {
                TermId = termId,
                Name = name,
                Status = status,
                StartDate = startDate,
                EndDate = endDate,
                AlertOn = alertOn,
                InstrName = instrName,
                InstrPhone = instrPhone,
                InstrEmail = instrEmail,
                Notes = notes
            };

            await conn.InsertAsync(course);
            //AUTO INCREMENTED ID AUTOMATIICALY GENERATED
            int id = course.Id;
        }

        public static async Task DeleteCourse(int id)
        {
            await Init();
            //await conn.DeleteAsync(id);
            await conn.DeleteAsync<Course>(id);
        }

        public static async Task<IEnumerable<Course>> GetCourses(int termId)
        {
            await Init();

            var courses = await conn.Table<Course>().Where(x => x.TermId == termId).ToListAsync();
            return courses;


        }

        public static async Task<IEnumerable<Course>>GetCourses()
        {
            await Init();

            var courses = await conn.Table<Course>().ToListAsync();
            return courses;
        }

        public static async Task UpdateCourse(int id, string name, string status, DateTime startDate, DateTime endDate, 
            bool alertOn, string instrName, string instrPhone, string instrEmail, string notes)
        {
            await Init();

            var courseQuery = await conn.Table<Course>().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (courseQuery != null)
            {
                courseQuery.Name = name;
                courseQuery.Status = status;
                courseQuery.StartDate = startDate;
                courseQuery.EndDate = endDate;
                courseQuery.AlertOn = alertOn;
                courseQuery.InstrName = instrName;
                courseQuery.InstrPhone = instrPhone;
                courseQuery.InstrEmail = instrEmail;
                courseQuery.Notes = notes;

                await conn.UpdateAsync(courseQuery);
          
            }

        }
        #endregion
        //Load Sample Data ---TERM----

        public static async void LoadSampleData()
        {
            await Init();

            Term term1 = new Term
            {
                Name = "Term 1",
                Status = "Current",
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Today.AddDays(3)
            };

            await conn.InsertAsync(term1);

            Term term2 = new Term
            {
                Name = "Term 2",
                Status = "Not Started",
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Today.AddDays(10)
            };

            await conn.InsertAsync(term2);
        }

        public static async Task ClearSampleData()
        {
            await Init();

            await conn.DropTableAsync<Term>();
            conn = null;

        }
    }

}
