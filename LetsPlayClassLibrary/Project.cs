using MySql.Data.MySqlClient;

namespace LetsPlayClassLibrary
{
    public class Project
    {
        // properties
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Budget { get; set; }

        // constructor
        public Project()
        {
            ProjectId = -1;
            UserId = -1;
            Title = string.Empty;
            Description = string.Empty;
            Media = "https://";
            StartDate = DateTime.MinValue;
            EndDate = DateTime.MinValue;
            Budget = 0;
        }

        // method to map db values to class properties 
        private static Project Mapper(MySqlDataReader reader)
        {
            Project prj = new Project();
            prj.ProjectId = reader.GetInt32("projectId");
            prj.UserId = reader.GetInt32("userId");
            prj.Title = reader.GetString("title");
            prj.Description = reader.GetString("description");
            prj.Media = reader.GetString("media");
            prj.StartDate = reader.GetDateTime("startdate");
            prj.EndDate = reader.GetDateTime("enddate");
            prj.Budget = reader.GetDecimal("budget");
            return prj;
        }

        // method to load list of projects
        public static List<Project> LoadProjects()
        {
            string query = "SELECT projectId,userId,title,description,media,startdate,enddate,budget FROM opensilverdb.projects ORDER BY title ASC";
            return DbHelper.LoadRecords<Project>(query, Mapper);
        }


        // method to load list of projects by User
        public static List<Project> LoadProjectsByUser(string userId)
        {
            string query = @"SELECT projectId,userId,title,description,media,startdate,enddate,budget FROM opensilverdb.projects pr INNER JOIN opensilverdb.users s ON pr.userId = s.userId WHERE pr.userId = @userId";
            MySqlParameter p = new MySqlParameter("@userId", userId);
            return DbHelper.LoadRecords<Project>(query, Mapper, p);
        }

        // method to save a project
        public void MakeProject()
        {
            string query = @"INSERT INTO opensilverdb.projects (projectId,userId,title,description,media,startdate,enddate,budget) 
                           VALUES (@projectId,@userId,@title,@description,@media,@startdate,@enddate,@budget)";
            MySqlParameter[] parameters = new MySqlParameter[8];
            parameters[0] = new MySqlParameter("@projectId", ProjectId);
            parameters[1] = new MySqlParameter("@userId", UserId);
            parameters[2] = new MySqlParameter("@title", Title);
            parameters[3] = new MySqlParameter("@description", Description);
            parameters[4] = new MySqlParameter("@media", Media);
            parameters[5] = new MySqlParameter("@startdate", StartDate);
            parameters[6] = new MySqlParameter("@enddate", EndDate);
            parameters[7] = new MySqlParameter("@budget", Budget);
            DbHelper.Execute(query, parameters);
        }

        // method to update a project
        public void Update()
        {
            string query = @"UPDATE opensilverdb.projects SET 
                        userId = @userId,
                        title = @title,
                        description = @description,
                        media = @media,
                        startdate = @startdate,
                        enddate = @enddate,
                        budget = @budget,
                        WHERE projectId = @projectId;";
            MySqlParameter[] parameters = new MySqlParameter[8];
            parameters[0] = new MySqlParameter("@projectId", ProjectId);
            parameters[1] = new MySqlParameter("@userId", UserId);
            parameters[2] = new MySqlParameter("@title", Title);
            parameters[3] = new MySqlParameter("@description", Description);
            parameters[4] = new MySqlParameter("@media", Media);
            parameters[5] = new MySqlParameter("@startdate", StartDate);
            parameters[6] = new MySqlParameter("@enddate", EndDate);
            parameters[7] = new MySqlParameter("@budget", Budget);
            DbHelper.Execute(query, parameters);
        }

        // method to delete a project
        public void DeleteProject()
        {
            string query = @"DELETE FROM opensilverdb.projects WHERE projectId = @projectId;";
            MySqlParameter parameter = new MySqlParameter("@projectId", ProjectId);
            DbHelper.Execute(query, parameter);
        }
    }
}
