![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=csharp&logoColor=white) [![Licence](https://img.shields.io/github/license/Ileriayo/markdown-badges?style=for-the-badge)](./LICENSE)

# GitLabGroup

Adds a bulk of Users to a Gitlab group.

## Build
```
dotnet build .
```

## Arguments
```
GitLabGroup.exe --url=[Gitlab-URL] --token=[PRIVATE-TOKEN] --group=[groupname] --permission=[Guest|Reporter|Developer|Maintainer|Owner] --file=[CSV-File]

Example:
GitLabGroup --url=https://mygitlab/api/v4/ --token=XYZ --group=grpDev --permission=Developer --file=input.csv
```

## Docker Support

### Build Image
```
docker build -t gitlabgroup .
```

### Run Container (example)
```
docker run --name gitlabdemo -it -v /c/temp/input.csv:/app/publish/input.csv gitlabgroup --url=https://mygitlab/api/v4/ --token=XYZ --group=grpTest --permission=Developer --file=/app/publish/input.csv
```
