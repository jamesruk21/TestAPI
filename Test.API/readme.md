Hi,
F5/running in debug is sufficient to run this project for the UI

It's based on .NET 6 so make sure you have that.

The API returns an array of person objects or a 400 if it doesn't like the request. See the validation in the controller.

I've split this into clean architecture like layers. At the moment, with the data being in a file, I just loaded this in teh PersonRepository, for each call. Ideally, you would have a separate layer here to load that data or call a database or whatever in a proper solution.
This needs to be running before running the UI project.

I've used SOLID principles here, so that it's easier to test, aside from the json doc as mentioned above for expediency, but that's not necessarily realistic anyway, the split in the Solution Explorer of layers should suffice to demonstrate that.

I've got tests covering the requirements too, so that should also be sufficent to cover off that.

I ended up with the search logic in the repository. Thinking about it, now there is essentially two pieces of logic in there, (the first and last name search, then the more generic search), I could have had the control logic for that in the service layer and the individual search types in the repo class. But its done now :-)

The relative path to the json data is in the appsettings.json file, and for the testing, I've coded it into the PersonRepositoryTest constructor

Thanks for your time and the interview.

I wish you all well whatever happens.

Regards
Jim