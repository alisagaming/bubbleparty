unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, GR32, GR32_PNG, StdCtrls;

type
  TForm1 = class(TForm)
    mmo1: TMemo;
    procedure FormCreate(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;

implementation

uses StrUtils;

{$R *.dfm}

const
//  fileName : string = 'BISpriteSheetGameplay1-ipadhd';
//  filesOutDir : string = 'out2';
//  fileName : string = 'gameHudAssets-hd';
  fileName : string = 'bubbleAssets-hd';

procedure TForm1.FormCreate(Sender: TObject);
var
  bitmap:TBitmap32;
  bitmapOut:TBitmap32;
  Png: TPortableNetworkGraphic32;
  PngOut: TPortableNetworkGraphic32;
  x,p:integer;
  fname,str,str2:string;
  px,py,dx,dy:Integer;
  textureRotatedFlag:Boolean;
begin
  Png:= TPortableNetworkGraphic32.Create;
  PngOut:= TPortableNetworkGraphic32.Create;

  Png.LoadFromFile(fileName+'.png');//'bubbleAssets-hd.png');
  bitmap := TBitmap32.Create;
  bitmap.Width := Png.Width;
  bitmap.Height := Png.Height;
  Png.DrawToBitmap32(bitmap);


  bitmapOut := TBitmap32.Create;

  textureRotatedFlag := false;

  mmo1.Lines.LoadFromFile(fileName+'.xml');//'bubbleAssets-hd.xml');
  for x:=0 to mmo1.Lines.Count-1 do
  begin
    p := PosEx('		<key>',mmo1.Lines[x]);
    if p = 1 then
    begin
      fname := Copy(mmo1.Lines[x], 8, Length(mmo1.Lines[x])-7-6);
    end;

    p := PosEx('<string>{{',mmo1.Lines[x]);
    if p > 0 then
    begin
      str := Copy(mmo1.Lines[x], p+10, Length(mmo1.Lines[x])-p-9-11);
      p := PosEx(',',str);
      str2 := Copy(str,0,p-1);
      str := Copy(str,p+1,Length(str)-p);
      px := StrToint(str2);

      p := PosEx('}',str);
      str2 := Copy(str,0,p-1);
      str := Copy(str,p+4,Length(str)-p-3);
      py := StrToint(str2);

      p := PosEx(',',str);
      str2 := Copy(str,0,p-1);
      str := Copy(str,p+1,Length(str)-p);
      dx := StrToint(str2);

      dy := StrToint(str);
    end;

    p := PosEx('<key>textureRotated</key>',mmo1.Lines[x]);
    if p>0 then textureRotatedFlag := True;

    p := PosEx('/>',mmo1.Lines[x]);
    if textureRotatedFlag and (p>0) then
    begin
      textureRotatedFlag := false;
      bitmapOut.Width := dx;
      bitmapOut.Height := dy;

      bitmapOut.Draw(-px, -py, bitmap);
      PngOut.Assign(bitmapOut);
      if(mmo1.Lines[x] = '			<false/>') then
        PngOut.SaveToFile(fileName+'\'+fname)
      else
        PngOut.SaveToFile(fileName+'\_'+fname);
    end;
  end;
end;

end.
