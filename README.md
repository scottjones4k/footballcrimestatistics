# footballcrimestatistics

## Running 
### Option 1: Visual Studio
1. Open FootballCrimeStatistics.sln in Visual Studio
2. Run project FootballCrimeStatistics
### Option 2: Docker
1. In the root of the git repository, run
```
docker build -t footballcrimestatistics:latest .
```
2. Run
```
docker run -p 8080:80 footballcrimestatistics:latest
```
Note: replace 8080 with any free port on your local machine


3. Navigate to http://localhost:8080/swagger/index.html
### Option 3: Prebuilt docker image
1. Run
```
docker run -p 8080:80 scottjones4k/footballcrimestatistics:latest
```
Note: replace 8080 with any free port on your local machine


2. Navigate to http://localhost:8080/swagger/index.html

## Tests
### Option 1: Visual Studio
1. Open FootballCrimeStatistics.sln in Visual Studio
2. Run all tests in solution

### Option 2: Docker
1. In the root of the git repository, run
```
docker build --target test .
```