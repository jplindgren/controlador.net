using Gerenciador.Domain;
using Gerenciador.Services.Data;
using System;
namespace Gerenciador.Services.Interface {
    public interface IProjectSummaryService {
        ProjectSummary GetProjectSummary(Guid id);
        void CreateProject(Project project);
    }// class
}
