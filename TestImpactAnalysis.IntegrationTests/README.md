I use special projects for tests, which make it easy to understand that the program is working correctly. 
To get the project, I use the `git clone` command inside the tests.
This was done because you cannot store git repositories inside another git repository(only if through a submodule, but this option does not suit me, since the submodule clearly defines a link to the parent git repository, and in tests the position of this submodule may change). 
Because of this, it is very important to check that the cloned repository has been deleted after the tests(otherwise you will not be able to make commits in the parent git repository)

List of repositories that I use for tests

- https://github.com/testimpactanalysis/SimpleProject