@echo off

"C:\Downloads\xmlstarlet-1.6.1\xml.exe" val --err --xsd "%cd%\shops.xsd" "%cd%\shops.xml"