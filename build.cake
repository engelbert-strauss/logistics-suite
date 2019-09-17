#tool "nuget:?package=GitVersion.CommandLine&version=5.0.1"
#addin "nuget:?package=Cake.Docker&version=0.10.1"


var target = Argument("target", "Build");

Task("Build")
	.Does(() =>
{
	var version = GitVersion();

	System.IO.File.WriteAllText(
		"./docker-compose.yml",
		System.IO.File.ReadAllText("./docker-compose-template.yml")
			.Replace("#{VERSION}#", version.FullSemVer));

	DockerComposeBuild(new DockerComposeBuildSettings
	{
		Parallel = true,
	});
});

Task("Deploy")
	.Does(() =>
{
	DockerCustomCommand("stack deploy --compose-file ./docker-compose.yml logisticssuite");
});

Task("Stop")
	.Does(() =>
{
	DockerCustomCommand("stack rm logisticssuite");
});

RunTarget(target);
