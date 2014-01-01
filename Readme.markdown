This project aims to provide a simple library for users to write bots for the [Virtual Paradise](http://virtualparadise.org/) platform. Bots are instances that interact with a world of Virtual Paradise. Example uses of bots are games, services, and building tools.

Currently, the only official method of interacting with Virtual Paradise is through the [C SDK](http://dev.virtualparadise.org/). Therefore, this library acts as a wrapper around native C methods. This project should not be confused with the [official .NET wrapper](http://vpnet.codeplex.com/); instead, this is an alternative to it.

## Known issues
Please be advised that due to a [NuGet bug](http://nuget.codeplex.com/workitem/3135), projects that use this library via NuGet should distribute the `packages` folder instead of relying on NuGet package restore.