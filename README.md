# Shakesmon
A simple web app which adds a Shakespeare flavor to pokemon descriptions. It exposes a REST API that, given a Pokemon name, returns its Shakespearean description.

It relies on two platforms:
- [PokeApi](https://pokeapi.co/) to fetch pokemons' description
- [FunTranslations](https://funtranslations.com/api/shakespeare) to translate an english text into the Shakespeare version.

## How to run it

### Standalone
Requirements:
- [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)

Clone the project on your local machine, navigate to the Shakesmon folder, open the command line and type:
```
dotnet run --project .\src\Avalentini.Shakesmon.Api\Avalentini.Shakesmon.Api.csproj
```
To test the application, open you browser and navigate to:
```
https://localhost:5001/api/pokemon/bulbasaur
```

You should see a response like the one below:
```JSON
{
  "name": "bulbasaur",
  "description": "Bulbasaur can beest seen napping in bright sunlight. Thither is a seed on its back. By soaking up the travelling lamp’s rays, the seed grows progressively larger."
}
```

### Docker
TBD

## LICENSE
Shakesmon is released under the MIT license.

Favicon provided by [draseart](http://www.iconarchive.com/artist/draseart.html) through [IconArchive](http://www.iconarchive.com/show/dumper-icons-by-draseart/PokeBall-icon.html).
