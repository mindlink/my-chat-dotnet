Programming Exercise
====================

This is a skeleton application to be used as part of a software development interview.

Instructions
------------

* Treat this code as if you owned this application, do whatever you feel is necessary to make this your own.
* There are several deliberate design, code quality and test issues that should be identified and resolved.
* We are looking for good code quality, pretty much everything in this skeleton represents code that we would not be happy with.
* Below is a list of the current features supported by the application; as well as additional features that have been requested by the business owner.
* In order to work on this take a fork into your own GitHub area; make whatever changes you feel are necessary and when you are satisfied submit back via a pull request. See details on GitHub's [Fork & Pull](https://help.github.com/articles/using-pull-requests) model
* Be sure to put your name in the pull request comment so your work can be easily identified.
* The project is written using .net 4.6.1, we encourage using the latest version of .net and C#, but this is your project now, so you are free to choose a different version.
* The project uses nuget to resolve dependencies however if you want to avoid nuget configuration the only external package is json.net.
* Refactor and add features (from the below list) as you see fit; there is no need to add all the features in order to "complete" the exercise.
* We will only consider candidates that implemented at least the "essential" features.
* Keep in mind that code quality is the critical measure and there should be an obvious focus on __TESTING__.
* REMEMBER: this is YOUR code, make any changes you feel are necessary.
* You're welcome to spend as much time as you like.
* The code will be a representation of your work, so it's important that all the code--new and pre-existing--is how you want your work to be seen.  Please make sure that you are happy with ALL the code.
* Oh yea, and the existing unit tests don't pass on purpose ;) that's up to you to fix/rewrite/delete as you deem fit.

#### Things We Value

* Good code structure - separation of concerns,
* A well-formed exception model,
* Tidy code,
* Application of appropriate design patterns,
* Unit tests.

#### Things To Avoid At All Costs

* Throwing general exception,
* Magic strings,
* Methods that do everything,
* No testing.

my-chat
-------

### Essential Features

* A user can export a conversation from a given file path stored in the following file format into a JSON file at the given output path:
```
<conversation_name><new_line>
(<unix_timestamp><space><username><space><message><new_line>)*
```
* Messages can be filtered by a specific user
    * The user can be provided as a command-line argument (how the argument is specified is up to you)
    * All messages sent by the specified user are written to the JSON output
    * Messages sent by any other user are not written to the JSON output
* Messages can be filtered by a specific keyword
    * The keyword can be specified as a command-line argument
    * All messages sent containing the keyword are written to the JSON output
    * Messages sent that do not contain the keyword are not written to the JSON output
* Hide specific words
    * A blacklist can be specified as a command-line argument
    * Any blacklisted word is replaced with "\*redacted\*" in the output.

### Additional Features

Implementing any of these features well will make your submission stand-out. Features listed here are ordered from easy to hard.

* Hide credit card and phone numbers
    * A flag can be specified to hide credit card and phone numbers
    * Any identified credit card or phone numbers are replaced with "\*redacted\*" in the output.
* Obfuscate user IDs
    * A flag can be specified to obfuscate user IDs
    * All user IDs are obfuscated in the output.
    * The same original user ID in any single export is replaced with the same obfuscated user ID i.e. messages retain their relationship with the sender, only the ID that represents the sender is changed.
* A report is added to the conversation that details the most active users
    * The most active user in a conversation is the user who sent the most messages.
    * Most active users are added to the JSON output as an array ordered by activity.
    * The number of messages sent by each user is included.
* A web-service is exposed in addition to a console application
    * The web-service allows the same options to be specified in form fields,
    * A conversation is uploaded as a file,
    * A JSON response is returned containing the transformed conversation.
