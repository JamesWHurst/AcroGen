2019/9/21
This application is intended for authoring an Acrobat document such that it can be generated from within Luviva.

To this end, we need a class to represent the document, which contains all of the information needed to render it.
Also, a 'generate code' function to output the C# code for generating it. That, or else the infor may be saved within a separate file.

Fields:
  Label (static text)
    text content
    size
    colon?
    left-margin (and can be set from the widest of some group of other labels + text-fields)
    centered?


  Text
    textual-content
    underline?
    distance the underline is below (global)
    text content for when empty or null
    left-margine (and can be set from the widest of some other group of labels)
    bold?
    preficed with a dash?

  Picture
    X-y position
    width and height
    picture-content

  text-entry field
    multi-line?
    position
    dimensions
    should any edge attach to a margin?
    label

margins
language

UI
  Major Panes:
  1. Hierarchical list of elements
  2. Design-Surface

