
    const compiled = () => {
        let __output = '';
        var fruits = ["Apple", "Pear", "Orange", "Lemon"]
  var random = " ".repeat(198).split("").map(x => Math.random());
 
__output += `
These fruits are amazing:
`; 
for(var i = 0; i < fruits.length; ++i) { 
__output += `
  - <b>`; 
__output += `${escape(fruits[i])}`; 
__output += `s</b>
`; 
}  
__output += `
Let's see some random numbers:
`; 
random.forEach((c, i) => {  
__output += `${escape(c.toFixed(10) + ((i + 1) % 6 === 0 ? "\n": "") )}`; 
__output += `The spaces before me should dissapear :O
`; 
}); 

        return __output;
    }