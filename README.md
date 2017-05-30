# Debug Reloaded 
Un piccolo debug.exe funzionante con i sistemi x64

## Comandi Inseriti 
* mov ax,bx
* mov bx,ax
* mov ax,cx
* mov cx,ax
* mov si,ax
* mov ax,si
* mov ax,i16
* mov bx,i16
* mov si,i16
* mov ax,mem16
* mov bx,mem16
* mov cx,mem16
* mov si,mem16
* mov m16,i16
* mov m16,ax
* inc ax
* inc bx
* inc cx
* inc si
* sub ax
* sub bx
* sub cx
* sub si
* add ax,i16
* add bx,ax
* add ax,bx
* add m16,bx
* sub ax,i16
* sub bx,ax
* sub ax,bx
* sub m16,bx
* jmp i16
* jnz i16
* cmp ax,bx

Legenda:

[tipo][lunghezza in bit]

i: valore immediato
f: flag
m: valore da memoria
r: registro

esempi:

          i16 => valore immediato da 16 bit (word) (0000, ABCD, 0666, etc..) 
          r16 => registro da 16 bit (word) (ax,bx,cx,si, etc..)
          m16 => valore in memoria da 16 bit (word) (word [0100], [0ABC], word [0300], etc..)
          f1 => qualsiasi flag (cf,pf, etc..)
          
          i8 => valore immediato da 8 bit (byte) (00, AB, 66, etc..) 
          r8 => registro da 16 bit (byte) (al,bh,cl, etc..) (non ancora impmentato)
          m8 => valore in memoria da 8 bit (word) (byte [0100], byte [0ABC], byte [0300], etc..) (non ancora impmentato)
          

### Verione database: 0.1

## TO-DO List

* Rendere disponibili AH,AL,BL, etc.. da 8 bit
* Differenziare tra byte e word
* Comandi mul,div,and,xor
* Comandi di salvataggio









