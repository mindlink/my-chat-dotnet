Solution for Mindlink Programming Exercise
====================

External packages used:
json.net, Command Line parser (available via Nuget or https://commandline.codeplex.com/)


my-chat
-------

### Essential Features
--------------------------------------------------
Task 1 [solved]
* Messages can be filtered by a specific user
    * The user can be provided as a command-line argument (how the argument is specified is up to you)
    * All messages sent by the specified user are written to the JSON output
    * Messages sent by any other user are not written to the JSON output

By providing the -f <user id> switch (see ReadConversation @ExportHandler.cs)
--------------------------------------------------

Task 2 [solved]
* Messages can be filtered by a specific keyword
    * The keyword can be specified as a command-line argument
    * All messages sent containing the keyword are written to the JSON output
    * Messages sent that do not contain the keyword are not written to the JSON output

By providing the -k <keyword> switch (see ReadConversation @ExportHandler.cs)
---------------------------------------------------
Task 3 [solved]
* Hide specific words
    * A blacklist can be specified as a command-line argument
    * Any blacklisted word is replaced with "\*redacted\*" in the output.

By providing the -L <word1,word2,...,wordn> switch (see RedactMessages @ExportHandler.cs)

=======================================================================
### Additional Features
=======================================================================

Task 1 [not solved]
* Hide credit card and phone numbers
    * A flag can be specified to hide credit card and phone numbers
    * Any identified credit card or phone numbers are replaced with "\*redacted\*" in the output.

Some issues occurred about the formatting of the credit card and the phone number

-------------------------------------------------------------------------------

Task 2 [solved]
* Obfuscate user IDs 
    * A flag can be specified to obfuscate user IDs
    * All user IDs are obfuscated in the output.
    * The same original user ID in any single export is replaced with the same obfuscated user ID i.e. messages retain their relationship with the sender, only the ID that represents the sender is changed.

By providing the -h switch 
Used MD5 encryption to encrypt user id's. Also all the user id's mentioned during the conversation 
were added to the redacted list.  (see idHash @User.cs)

-------------------------------------------------------------------------------
Task 3 [solved]
* A report is added to the conversation that details the most active users
    * The most active user in a conversation is the user who sent the most messages.
    * Most active users are added to the JSON output as an array ordered by activity.
    * The number of messages sent by each user is included.

By providing the -r switch (See WriteConversation @ExportHandler.cs)

----------------------------------------------------------------------------------
Task 4 [not solved]
* A web-service is exposed in addition to a console application
    * The web-service allows the same options to be specified in form fields,
    * A conversation is uploaded as a file,
    * A JSON response is returned containing the transformed conversation.
