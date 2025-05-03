# AssemblyMiner

Use docker to build the image.

```sh
docker build -t assemblyminer .
```

The intent usage is to run 

```sh 
version=$(docker run --rm -v /mnt/c/Temp/TestGit.dll:/data/TestGit.dll assemblyminer --path /data/TestGit.dll)
```
to obtain informational version directly from the assembly. 

Yet the docker image labels added during the image build turned to be more of a use. 

---
Created from [JandaBox](https://github.com/Jandini/JandaBox)
