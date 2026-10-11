# Shaders / Shader Includes

## How to use

These shader and shader include files are provided as a part of the package, and should be visible from within the solution. However, Godot won't venture beyond the project directory when referencing resources, thus these must be copied into the project folder (`res://`) somewhere in order to be utilized.

#### Shader Includes

Files with the extension `.gdshaderinc` are files that include helpful shader functions. They should be included with a preprocessor directive.

e.g.:

```gdshader
shader_type canvas_item;

#include "res://shaders/useful-junk.gdshaderinc"

void fragment() {
[...]
```

- [#include directive](https://docs.godotengine.org/en/stable/tutorials/shaders/shader_reference/shader_preprocessor.html#include)
- [ShaderInclude Resource](https://docs.godotengine.org/en/stable/classes/class_shaderinclude.html#class-shaderinclude)