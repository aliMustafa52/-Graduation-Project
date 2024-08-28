namespace graduationProject.Api.Errors
{
	public static class ProjectsErrors
	{
		public static readonly Error ProjectNotFound =
			new("Project.NotFount", "No Project was found with this Id", StatusCodes.Status404NotFound);

		public static readonly Error DublicatedProject
				= new("Project.Dublicated", "Cannot create a Dublicated Project", StatusCodes.Status409Conflict);

		public static readonly Error CannotEditOthersProjects
				= new("Project.CannotEditOthersProjects", "Cannot Edit Others Projects", StatusCodes.Status400BadRequest);

		public static readonly Error CannotDeleteOthersProjects
				= new("Project.CannotDeleteOthersProjects", "Cannot Delete Others Projects", StatusCodes.Status400BadRequest);

		public static readonly Error ProjectAssignedOrCompleted
				= new("Project.ProjectAssignedOrCompleted", "Project Assigned to another provider Or Completed", StatusCodes.Status400BadRequest);
	}
}
