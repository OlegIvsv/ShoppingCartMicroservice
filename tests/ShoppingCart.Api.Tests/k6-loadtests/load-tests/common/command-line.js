
export function readCommandLineParam(key){
    if(key in __ENV)
        return __ENV[key];
    console.error(`Seems like the parameter '${key}' wasn't specified`);
    throw new Error(`Not all params were specified`);
}
