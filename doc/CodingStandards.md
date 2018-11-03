# Coding Standards

This document covers all things about naming variables, formatting code, pull requests and process for commiting and merging.

## Naming

### Classes and Enums

Use `UpperCamelCase` for classes and enums. This also strictly applies to abbreviations.

Examples:

```csharp 
public class CameraTool { ... }
```
```csharp
public class CpuProfiler { ... }
```
```csharp
enum EnemyAiState { NormalPatrol, Investigate, Alert };
```

### Member Variables

Use `UpperCamelCase` for **public** member variables.

Use `lowerCamelCase` for **private** member variables.

Use `_lowerCamelCase` for **protected** member variables.

These also apply to abbreviations.

Example:

```csharp
class CpuProfiler
{
    public string CpuModel;
    private int startTimeStamp;
    protected float _cpuClockRate;
}
```

### Memeber Methods

Use `UpperCamelCase` for all member methods.

Example:

```csharp
class CpuProfiler
{
    private float CalculateCpuUsage() { ... }
    public void OutputCpuModel() { ... }
}
```

### Other Variables

Use `lowerCamelCase` for temporary variables and function parameters.

Use `UPPER_CASE_UNDERSCORE` for constants.

Example:

```csharp
class CpuProfiler
{
    private float CpuUtilization(int currentSpeed)
    {
        const long BASE_SPEED = 4294967296;
        float utilizationPercentage;
        ...
    }
}
```

### Other

Use `UpperCamelCase` to name objects in Unity Editor.

Use `Upper-Case-Hyphen` to name a feature branch.

## Formatting

Use default settings of Visual Studio for C# language. You can find these settings in `Tools` - `Options` - `Text Editor` - `C#`.

Use `Ctrl + K, Ctrl + D` in Visual Studio to format the entire document, and `Ctrl + K, Ctrl + F` to format selection with these settings.

## Pull Request

The comment of a pull request must include:

 * URL of the Trello story related to this pull request;
 * Story to be tested in [Test Plan Document](TestPlan/TestPlanDoc.md);
 * Functionality or bug fixed of this pull request;
 * Testing instructions;
 * Notes if necessary.

Example:

> ## Camera Tool
> Trello URL: https://trello.com/c/QkUjj2H1/43-camera-tool-5-points
>
> Story to be tested: _Camera Tool_
> 
> Functionality:
>
> * The drone can take photos of top secret info and get score with this tool.
>
> Testing Instructions:
> 1. ...
> 2. ...
>
> **Notes**: ...

## Code Review Process

### For author of the code

1. Commit the change;
2. Change the story test status in [Test Plan Document](TestPlan/TestPlanDoc.md) from **UNTESTED** to **READY FOR TEST** on **branch `Test-Doc`**; don't open a new branch for this;
3. Fill the `Procedure` of the related story in [Test Plan Document](TestPlan/TestPlanDoc.md) on **branch `Test-Doc`**; don't open a new branch for this;
4. Before opening a pull request, check if everything done is checked in Trello;
5. Open a pull request with all necessary information; resolve the conflict if necessary;
6. Include the URL of the pull request in related Trello story;
7. After the pull request is reviewed and approved, merge the pull request yourself;
8. Any pull request that doesn't follow this process will be closed.

### For reviewers

1. Read the pull request carefully;
2. Read the code and see if there's anything that needs to be changed;
3. If there are problems in the code, comment and request change;
4. Request change if the Trello status is not the same;
5. Test the functionality of the code; change the story test status in [Test Plan Document](TestPlan/TestPlanDoc.md) from **READY FOR TEST** to **PASSED** or **FAILED** and include your initials and test date on **branch `Test-Doc`**; don't open a new branch for this. Since we need two reviewers on one pull request, we need two tests on each story. Include your test result after the previous reviewers' result. It's possible to have two different results. For example: 
  > _Camera Tool_: __PASSED__. MD 11/2; __FAILED__. VS 11/2
6. If the test failed, fill the `Notes` and comment in the pull request;
7. If the test passed, approve the pull request;
8. Any review that doens't follow this process will be dismissed.

Branch `Test-Doc` will be merged into `master` after all stories in the sprint pass the test before the end of the sprint.