
Programming Exercise
====================


At this point, The MyChat application can do the Following things:


### Essential Features

* A user can export a conversation from a given file path stored in the following file format into a JSON file at the given output path:
```
<conversation_name><new_line>
(<unix_timestamp><space><username><space><message><new_line>)*
```
The paths for the InputFile and the outputFile are transmited as Command-line Arguments
The user is asked next in the command-line if he wants to Filter the messages. 

* Messages can be filtered by a specific user
    * The user is provided as a command-line argument 
    * All messages sent by the specified user are written to the JSON output
    * Messages sent by any other user are not written to the JSON output
* Messages can be filtered by a specific keyword
    * The keyword is specified as a command-line argument
    * All messages sent containing the keyword are written to the JSON output
    * Messages sent that do not contain the keyword are not written to the JSON output
* Hide specific words
    * A blacklist can be constructed by writing the path to a file that contains a list of words as a parameter or it can be contructed by sending a list of words as a parameter.
    * Any blacklisted word is replaced with "\*redacted\*" in the output.

### Additional Features
* Hide credit card and phone numbers

    The messages are analyzed using regex for credit card numbers or phone numbers.
    A phone number is a string of numbers, 7 to 10 digits long with spaces or hyphens anywhere in beetween.
    A credit card number, similar to phone numbers should be a string of numbers, 13 to 16 digits long with spaces or hyphens anywhere in beetween.
    * Any identified credit card or phone numbers are replaced with "\*redacted\*" in the output.
