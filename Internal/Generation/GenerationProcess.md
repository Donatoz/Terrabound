# Generation process

1. First of all, world generator needs to generate the world generation meta.
This meta represents a serialized world which can be saved.
    * To generate a world, generator needs to be provided generation settings.
    * Generation settings represent a cluster of rules, which are resolved during main generation process.
    * __Rule__ - a generation member which is called between pre-process and post-process invocations.
      Fore example, rules (`BlockPlacementRule`) can be used to place the blocks according to the generated placement data.
    * Before and after resolving all rules, generator starts generation processes which prepare the generation data
    and polish generation result.
    * Each process must be given a generation meta-data piece - a piece of generation settings.
    * __Process__ has `Generate()` function, which modifies (overrides) 
      some of the final generation result values, therefore all processes must be somehow ordered.
2. Next, the generator creates a world using provided meta.