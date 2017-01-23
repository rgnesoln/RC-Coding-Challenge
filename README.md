# RC-Coding-Challenge
A console application that scans log files of a specific format for a
fault event pattern and counts the instances of fault.

A fault event pattern is where the unit is in stage 3 for 5 or more
minutes and then goes to stage 2.  The unit can flip between stages
2 and 3 any number of times, but then goes to stage 0 indicating
a positive fault event pattern, where it gets counted.

The input for the application is a log file that gets scanned and 
the output is a count of the number of fault event patterns detected.

In the Test folder, there are 3 test log files.  The expected results
for:
test01.log - 0 faults
test02.log - 1 fault
test03.log - 2 faults
