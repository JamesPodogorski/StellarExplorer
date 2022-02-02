# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.183.0/containers/ubuntu/.devcontainer/base.Dockerfile

# FROM ubuntu:latest as base_dep
# FROM mcr.microsoft.com/dotnet/sdk:latest
FROM mcr.microsoft.com/dotnet/sdk:6.0

RUN apt-get update && DEBIAN_FRONTEND=noninteractive \
    apt-get install -y vim git wget curl sudo apt-transport-https bash-completion

ARG USERNAME=vscode
ARG PASSWORD=vscode

# # Add user
RUN useradd -m -d /home/$USERNAME -s /bin/bash -c "the $USERNAME user" -U $USERNAME
RUN echo ${USERNAME}:${PASSWORD} | chpasswd
RUN adduser ${USERNAME} sudo
ENV HOME /home/${USERNAME}
WORKDIR /home/${USERNAME}
USER ${USERNAME}
