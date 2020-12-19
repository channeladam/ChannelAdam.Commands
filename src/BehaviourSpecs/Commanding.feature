Feature: Commanding

#Scenario: Should execute a command successfully
#
#Scenario: Should make available the exception that occurred during the execution of a command
#
#Scenario: Should make available the successful result from executing the command
#
#Scenario: Should only execute the command if the command is flagged as being allowed to be executed

Scenario: Should support reversing a command
Given an initial state to perform commands on
When a series of commands are executed
And all the commands are undone
Then the initial state is restored
