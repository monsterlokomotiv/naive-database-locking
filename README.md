# naive-database-locking

Library for using a form of distributed locking from .NET applications.

This is a 'naive' in the terms of performing the actual locking and synchronization itself very little and instead relying much on the underlying storages. 

The initial idea is to support relational databases such as PostGreSQL or MS SQL Server as they have built-in locking mechanisms when inserting rows. 
