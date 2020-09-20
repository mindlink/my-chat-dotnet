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
    
### Important
    * Multiple filters can be used at once, providing you follow the documented method/layout for the filters
    * How to use the filters are documented below

### Export a conversation
* To export enter the executable and pass some arguments next to it e.g. "YourDirectory/MyChat.exe example.txt example.json"
```
<conversation_name><new_line>
(<unix_timestamp><space><username><space><message><new_line>)*
```
### Messages can be filtered by a specific user
    * To filter by a user, do the usual to export a conversation and add the keywords "-filter-user" and then the user to filter by "bob"
    * Roughly it will look like "YourDirectory/MyChat.exe example.txt example.json -filter-user bob"
### Messages can be filtered by a specific keyword
    * TTo filter/search the chat based on a specific keyword do the following "YourDirectory/MyChat.exe example.txt example.json -filter-search-word pie"
### Hide specific words
    * To blacklist a word, you can either do a single word or multiple at once
    * A single word to blacklist do the following "YourDirectory/MyChat.exe example.txt example.json -filter-blacklist-word pie"
    * Multiple words to blacklist do the following "YourDirectory/MyChat.exe example.txt example.json -filter-blacklist-word pie|society" 
    * Adding the "|" between words instead of a space

### Additional Features

Implementing any of these features well will make your submission stand-out. Features listed here are ordered from easy to hard.

### Hide credit card and phone numbers
    * To hide credit cards do the following "YourDirectory/MyChat.exe example.txt example.json -filter-card-details"
    *To hid phone numbers do the following "YourDirectory/MyChat.exe example.txt example.json -filter-phone-number"
### Obfuscate user IDs
    * To obfuscate user IDs do the following "YourDirectory/MyChat.exe example.txt example.json -filter-obfuscate"
### A report is added to the conversation that details the most active users
    * The report is automatically generated for a conversation, so no need to input anything extra
* A web-service is exposed in addition to a console application
    * The web-service allows the same options to be specified in form fields,
    * A conversation is uploaded as a file,
    * A JSON response is returned containing the transformed conversation.
