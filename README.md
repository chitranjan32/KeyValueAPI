# KeyValueAPI
.NET Core API to Fetch , Create & Update KeyValue Pairs in In-Memory Database


=================================================================
NOTE : 

If Units Tests are not runnable , Please Uninstall and Re-Install below Nuget package in KeyValueAPITest Project 

 xunit.runner.visualstudio


===========Problem statement ============

Create basic RESTful API allowing to add, modify and read key and value pairs:

o    GET /{key} - returns the value for a given key

o    POST /{key}  with a value as body – adds value with the supplied key

o    PUT /{key}  with value as body – updates value for the supplied key

·         Keys must be url-safe (only alphanumeric, hyphen, period, underscore, tilde), treated as case-insensitive and limited to 32 characters in size.

·         Value is any text up to 1024 characters. 
