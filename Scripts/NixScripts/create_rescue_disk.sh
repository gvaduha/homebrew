#! /usr/bin/env bash
#
# Make bootable device
#
# busybox
#modprobe loop
#losetup /dev/loop0 file.iso
#mount /dev/loop0 /mnt/iso -t iso9660

device=/dev/sdc
partnum=2
part=$device$partnum

echo "Installing to $part"

#ensudshure it's Linux partition (0x83)
echo "t
$parnum
83
w" | fdisk $device

mkfs.ext4 $part

mount $part /mnt

#deviceid=`ls -l /dev/disk/by-uuid/ | grep "$device" | awk '{ print $9; }'`

# force in case of "do not proceed with blocklists"
sudo grub-install --force --root-directory=/mnt $part

# write grub.cfg
cat > /mnt/boot/grub/grub.cfg << ENDOFGRUB

if loadfont /boot/grub/font.pf2 ; then
set gfxmode=auto
insmod efi_gop
insmod efi_uga
insmod gfxterm
terminal_output gfxterm
fi

set menu_color_normal=white/black
set menu_color_highlight=black/light-gray

set default=0
set timeout=10

#menuentry "Try Lubuntu without installing" {
#set gfxpayload=keep
#linux/casper/vmlinuz.efi  file=/cdrom/preseed/lubuntu.seed boot=casper quiet splash ---
#initrd/casper/initrd.lz
#}

# need to find UUID of partition with blkid
#menuentry "Windows 7 (loader) (on /dev/sda1)" --class windows --class os {
#    insmod part_msdos
#    insmod ntfs
#    insmod ntldr
#    set root='(hd0,msdos1)'
#    search --no-floppy --fs-uuid --set=root 1EA0019AA0017A13
#    ntldr ($root)/bootmgr
#}

menuentry "Lubuntu" {
  loopback loop (usb0,$partnum)/isos/lubuntu.iso
  linux (loop)/casper/vmlinuz.efi boot=casper iso-scan/filename=/isos/lubuntu.iso noprompt
  initrd (loop)/casper/initrd.lz
}

menuentry "GParted Live" {
  loopback loop (usb0,$partnum)/isos/gparted.iso
  linux (loop)/live/vmlinuz boot=live iso-scan/filename=/isos/gparted.iso
#noswap noeject username=user union=overlay config components quiet ip= net.ifnames=0
  initrd (loop)/live/initrd.img
}

ENDOFGRUB

mkdir /mnt/isos

umount $part

echo "Done"
