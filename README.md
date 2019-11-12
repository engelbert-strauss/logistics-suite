Logistics Suite
===============

This project contains a small suite of different micro- / nanoservices that demonstrates the technology landscape at the company [engelbert strauss GmbH & Co. KG](https://www.engelbert-strauss.de).

The main purpose of this code base is to showcase it at the ITCS Frankfurt / Germany on 3rd December 2019. So that the
applicants can have a glance at the type of software being produced in the company.

Features
--------
- small _ASP.NET Core_ web services for each abstract company department
- simple _Angular_ web ui to display the communication flow of the abstract departments
- communication via different technologies
  - distributed caching (_Redis_)
  - message bus (_RabbitMq_)
  - persistence layer (_MongoDB_)
- containerized build (_Docker_)
- `docker stack` based _Kubernetes_ deployment

Requirements
------------
- _.NET Core Runtime_ with tools support
- _Docker_
- _Kubernetes_ single-node cluster via Docker Desktop

Getting Started
---------------

### Build

To build the different versioned containers via `docker-compose` execute the following statement inside a _PowerShell_.

~~~powershell
./build --target=build
~~~

### Deploy

After you built the current state of the repository into local containers you can deploy the composed stack to the local _Kubernetes_ cluster with the following _Powershell_ statement.

~~~powershell
./build --target=deploy
~~~

Now you can open the simple web ui of the _Logistics Suite_ on http://localhost:6060.

### Stop

Just issue the following statement in a _PowerShell_ to remove the deployed stack from the local _Kubernetes_ cluster.

