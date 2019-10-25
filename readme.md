My Chat - Cian Collier
====================

Basic Usage
------------
```
<path to my-chat executable> input.txt output.json
```
Input and output files are given as command line arguments as demonstrated 
above. It is essential that these arguments are provided. 

Filtering By User
-----------------
```
<path to my-chat executable> input.txt output.json -u userid 
```
To only include messages from a specific user use the -u flag. 
Replace the userid with the ID of the user whose messages you 
want to see in the output file. 

Filtering By Keyword
---------------------
```
<path to my-chat executable> input.txt output.txt -k keyword
```
Select only messages containing a given keyword. Replace 
keyword in the above demonstration command with the keyword 
your desired keyword. 

Blacklisting Words
-------------------
```
<path to my-chat executable> input.txt output.txt -b specify,blacklisted,words,like,this
```
Use the -b flag to filter out messages containing certain words.
The words can be specified as a comma separated list as above. 

Hiding Credit Card And Phone Numbers
-------------------------------------
```
<path to my-chat executable> input.txt output.json -hn
```
Specify the -hn flag to replace phone and credit card numbers
with *redacted* in the output file. 

Hiding User IDs
-------------------------------
```
<path to my-chat executable> input.txt output.json -ouid 
```
Specify the -ouid flag to replace each user ID with a generic
ID (i.e "Hidden User 1", "HIdden User 2" etc...).
