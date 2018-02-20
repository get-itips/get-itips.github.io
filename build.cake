#tool "nuget:?package=Wyam"
#addin "Cake.Powershell"

var RootDir = MakeAbsolute(Directory(".")); 
var buildtarget = Argument("target", "Default");
var waymexe = RootDir + "/tools/Wyam.1.3.0/tools/net462/Wyam.exe";

Task("Build")
    .Does(() => StartProcess(waymexe, "build"));

Task("Preview")
    .Does(() => StartProcess(waymexe, "-p -w"));

Task("Deploy")
    .IsDependentOn("Build")
    .Does(() => 
    {
        if(FileExists("./CNAME"))
            CopyFile("./CNAME", "output/CNAME");

        StartProcess("git", "checkout master");
        StartProcess("git", "add .");
        StartProcess("git", "commit -m \"Checking output in for subtree\"");
        StartProcess("git", "subtree split --prefix output -b gh-pages");
        StartProcess("git", "push -f origin gh-pages:gh-pages");
        StartProcess("git", "branch -D gh-pages");

    });

RunTarget(buildtarget);