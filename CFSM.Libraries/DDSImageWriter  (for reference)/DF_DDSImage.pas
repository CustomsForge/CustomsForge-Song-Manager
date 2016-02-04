library DF_DDSImage;

{$mode objfpc}{$H+}

{$R *.res}

uses
  SysUtils,
  Classes,
  ImagingTypes,
  Imaging,
  ImagingUtility;

function ConvertToDDS(source:Pointer;var dest :Pointer; sourceSize:integer):integer;stdcall;
var
   Image: TImageData;
   ms:TMemorystream;
begin
  result := 0;
  InitImage(Image);
  if (Imaging.LoadImageFromMemory(source,sourceSize,Image)) then
  begin
      ms := TMemorystream.create();
      Imaging.SaveImageToStream('dds',ms,Image);
      GetMem(dest,ms.size);
      Move(ms.Memory^, dest^, ms.size);
      result := ms.size;
      ms.free;
  end;
end;

procedure FreeDDS(source:Pointer);stdcall;
begin
  FreeMem(source);
end;


exports ConvertToDDS,FreeDDS;

end.

