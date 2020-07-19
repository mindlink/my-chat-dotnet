My-Chat Programming Exercise
====================

My-Chat enables you to import, filter and write to JSON a conversaton stored in the following file format:
```
<conversation_name><new_line>
(<unix_timestamp><space><username><space><message><new_line>)*
```

A user activity report is appended to each conversation, recording each user by message count in addition to the most active user.

Core Functionality
------------
#### Export a conversation
```<executable-path> input.txt output.json```

Filters
------------
Multiple filters may be applied to a single operation.
#### Filter conversation by user
```<executable-path> input.txt output.json -uf bob```

Only messages send by the filtered user will be displayed.

#### Filter conversation by keyword
```<executable-path> input.txt output.json -kf pie```

Only messages containing the filtered keyword will be displayed.

#### Filter conversation by with keyword blacklist
```<executable-path> input.txt output.json -kb pie```

```<executable-path> input.txt output.json -kb pie,society,buying```

All instances of blacklisted words will be replaced with `**redacted**`.

#### Hide credit card numbers
```<executable-path> input.txt output.json -hcc```

All instances of credit card numbers will be replaced with `**redacted**`.

#### Hide phone numbers
```<executable-path> input.txt output.json -hpn```

All instances of phone numbers will be replaced with `**redacted**`.

#### Obfuscate user IDs
```<executable-path> input.txt output.json -ou```

Users IDs will be replaced with a unique integer. Obfuscated user IDs remain unique to users.
