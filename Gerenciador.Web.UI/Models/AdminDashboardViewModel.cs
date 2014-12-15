using Gerenciador.Domain;
using Gerenciador.Domain.UserContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gerenciador.Web.UI.Models {
    public class AdminDashboardViewModel {
        public IList<Project> LastActivesProjects { get; set; }
        public int NumberActiveProjects { get; set; }

        public IEnumerable<TaskViewModel> NextTasks { get; set; }
        public IList<UserProfile> Users { get; set; }

        //When create an RegularUserDashBoard, remember to create an BaseUserDashboardViewModel and put this property to there
        public UserProfile CurrentUser { get; set; }

        public int NumberOfNewMessages { get; set; }
        public int NumberOfTickets { get; set; }
        public int NumberOfProposals { get; set; }
    } //class
}