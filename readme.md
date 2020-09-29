Programming Exercise
====================

This repository contains a skeleton application to be used as part of the MindLink software development interview.

Application
-----------

### Overview

This application is meant to read a conversation from a plain text file and output the conversation as JSON.

We are looking for the application to be fixed so that it works as intended and extended to support the functionality described below.

### Features

* A user can export a conversation from a given file path stored in the following file format into a JSON file at the given output path:
```
<conversation_name><new_line>
(<unix_timestamp><space><username><space><message><new_line>)*
```
* Messages can be filtered by a specific user
    * The user can be provided as a command-line argument `--filterByUser <user>`
    * All messages sent by the specified user appear in the JSON output
    * Messages sent by any other user do not appear in the JSON output
* Messages can be filtered by a specific keyword
    * The keyword can be specified as a command-line argument `--filterByKeyword <keyword>`
    * All messages sent containing the keyword appear in the JSON output
    * Messages sent that do not contain the keyword do not appear in the JSON output
* Hide specific words
    * A blacklist can be specified as a command-line argument `--blacklist <word1>,<word2>`
    * Any blacklisted word is replaced with "\*redacted\*" in the JSON output.
* Include a report of the number of messages each user sent
    * Adding the report to the output can be specified as a command-line argument `--report`
    * The report is sorted in descending order of number of messages sent
    * The report should appear in the final JSON output under the `activity` property:
    ```
    {
     "name": "...",
     "messages": [...],
     "activity": [
        {
            "sender": "...",
            "count": 100
        },
        ...]
	}
    ```

### Building and running

* Visual Studio - solution can be built, run and tested within Visual Studio

* Command line with dotnet Core SDK
    - To build everything
        `dotnet build` from within the solution directory
    - To run the application
        `dotnet run --project MyChat -- --inputFilePath <input> --outputFilePath <output>` from within the solution directory
    - To run the unit tests
        `dotnet test` from within the solution directory


Instructions
------------

1. Think about your architecture, maybe even sketch out a UML diagram!

2. Fork the repository into your own GitHub area

3. Install the relevant .net core SDK from https://dotnet.microsoft.com/download - you want at least .net core 3.1

4. Identify and fix the existing issue

5. Make whatever changes you feel are necessary, it's your code now!

6. Make sure all features are implemented, the code works and all the tests you wrote pass!

7. When you are satisfied, submit back via a pull request. See details on GitHub's [Fork & Pull](https://help.github.com/articles/using-pull-requests) model

8. Notify our recruitment team at `careers <at> mindlinksoft.com` with a link to your pull request and your real name ;)

### What we are looking for

* Application of SOLID principles - https://en.wikipedia.org/wiki/SOLID
    * Imagine that there are another 20 features to implement, design with that in mind!
    * DON'T put everything in one method/one class
    * DO think about **abstractions** and commonality between features
    * DO think about separating creating and "wiring-up" objects from using objects

* A well-formed exception model
    * DON'T `throw new Exception("non-descript message here")`
    * DO throw appropriate exceptions e.g. is an argument `null` and shouldn't be? `ArgumentNullException` then!
    * DO create and throw your own exception types **when it makes sense**
    * DO include inner exceptions, otherwise you lose your stack!

* Tidy code
    * Well named variables and methods
    * Consistent style

* Application of appropriate design patterns
    * There is scope to approach this challenge in different ways and design patterns can help you!

* Unit tests
    * DO write tests!
    * DO isolate your units under test (you don't need to read and write the file to test filtering behaviour if you have designed it well!)
    * DO include some end-to-end tests
