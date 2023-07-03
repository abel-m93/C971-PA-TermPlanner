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

        public static async Task UpdateCourse(int courseId, string name, string status, DateTime startDate, DateTime endDate, 
            bool alertOn, string instrName, string instrPhone, string instrEmail, string notes)
        {
            await Init();

            var courseQuery = await conn.Table<Course>().Where(x => x.Id == courseId).FirstOrDefaultAsync();

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

        /*DATABASES SERVICES FOR ASSESSMENTS*/

        #region ASSESSMENT DATABASE SERVICES
        public static async Task AddAssessment(int courseId, string name, string type, DateTime startDate, DateTime dueDate, bool alertOn)
        {
            await Init();

            Assessment assessment = new Assessment()
            {
                CourseId = courseId,
                Name = name,
                Type = type,
                StartDate = startDate,
                DueDate = dueDate,
                AlertOn = alertOn
                
            };

            await conn.InsertAsync(assessment);
            int id = assessment.Id;
        }

        public static async Task UpdateAssessment(int assessmentId, string name, string type, DateTime startDate, DateTime dueDate, bool alertOn)
        {
            await Init();

            var assessQuery = await conn.Table<Assessment>().Where(x => x.Id == assessmentId).FirstOrDefaultAsync();

            if (assessQuery != null)
            {
                assessQuery.Name = name;
                assessQuery.Type = type;
                assessQuery.StartDate = startDate;
                assessQuery.DueDate = dueDate;
                assessQuery.AlertOn = alertOn;

                await conn.UpdateAsync(assessQuery);
            }

        }

        public static async Task DeleteAssessment(int id)
        {
            await Init();
            await conn.DeleteAsync<Assessment>(id);
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments(int courseId)
        {
            await Init();
            var assessments = await conn.Table<Assessment>().Where(x => x.CourseId == courseId).ToListAsync();
            return assessments;
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments()
        {
            await Init();

            var assessments = await conn.Table<Assessment>().ToListAsync();
            return assessments;
        }



        #endregion

        #region METHODS FOR SAMPLE DATA


        public static async Task LoadSampleData()
        {
            await Init();

            Term sampleTerm1 = new Term
            {
                Name = "Term 1",
                Status = "Current",
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Today.AddMonths(6)
            };

            await conn.InsertAsync(sampleTerm1);

            Course sampleCourse1 = new Course
            {
                TermId = sampleTerm1.Id,
                Name = "C100 Intro to Humanities",
                Status = "Passed",
                StartDate = DateTime.Today.Date,
                EndDate = DateTime.Today.Date.AddDays(15),
                AlertOn = true,
                InstrName = "Abel Martinez",
                InstrPhone = "773-551-3861",
                InstrEmail = "amar815@wgu.edu",
                Notes = "Presentation Required"
            };

            await conn.InsertAsync(sampleCourse1);

            Assessment sampleAssessment1 = new Assessment
            {
                CourseId = sampleCourse1.Id,
                Name = "Intro to Humanities OA",
                Type = "Objective",
                StartDate = DateTime.Today.Date,
                DueDate = DateTime.Today.Date.AddDays(10),
                AlertOn = true
            };

            await conn.InsertAsync(sampleAssessment1);

            Assessment sampleAssessment2 = new Assessment
            {
                CourseId = sampleCourse1.Id,
                Name = "Intro to Humanities PA",
                Type = "Performance",
                StartDate = DateTime.Today.Date,
                DueDate = DateTime.Today.Date.AddDays(14),
                AlertOn = true
            };

            await conn.InsertAsync(sampleAssessment2);
           
         
        }

        public static async Task ClearSampleData()
        {
            await Init();

            await conn.DropTableAsync<Term>();
            await conn.DropTableAsync<Course>();
            await conn.DropTableAsync<Assessment>();
            conn = null;

            
            
        }
        #endregion
    }

}
