Imports System
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Module Program
    Sub Main(args As String())

        Dim key As String = "9999ABCDE-1234-aB1cD-1234561631700831102"
        Dim method As String = "aes-256-ecb"
        Dim data As String = "9999ABCDE-1234-aB1cD-1234561631700831102*********"

        key = LongKey(key, 32)

        Dim encryptedData As String = EncryptData(data, key, method)
        Console.WriteLine("Encrypted data : " + encryptedData)

        Dim decryptedData As String = DecryptData(encryptedData, key, method)
        Console.WriteLine("Decrypted data : " + decryptedData)
    End Sub

    Function EncryptData(data As String, key As String, method As String) As String
        'Dim aes As New AesManaged()
        Dim aes As Aes = Aes.Create()
        aes.Key = Encoding.UTF8.GetBytes(key)
        aes.Mode = CipherMode.ECB
        aes.Padding = PaddingMode.PKCS7

        Dim encryptor As ICryptoTransform = aes.CreateEncryptor()

        Dim ms As New MemoryStream()
        Using cs As New CryptoStream(ms, encryptor, CryptoStreamMode.Write)
            Dim dataBytes As Byte() = Encoding.UTF8.GetBytes(data)
            cs.Write(dataBytes, 0, dataBytes.Length)
        End Using

        Dim encryptedBytes As Byte() = ms.ToArray()
        Return Convert.ToBase64String(encryptedBytes)
    End Function

    Function DecryptData(data As String, key As String, method As String) As String
        'Dim aes As New AesManaged()
        Dim aes As Aes = Aes.Create()
        aes.Key = Encoding.UTF8.GetBytes(key)
        aes.Mode = CipherMode.ECB
        aes.Padding = PaddingMode.PKCS7

        Dim decryptor As ICryptoTransform = aes.CreateDecryptor()

        Dim encryptedBytes As Byte() = Convert.FromBase64String(data)
        Dim ms As New MemoryStream(encryptedBytes)

        Using cs As New CryptoStream(ms, decryptor, CryptoStreamMode.Read)
            Dim decryptedBytes(encryptedBytes.Length - 1) As Byte
            Dim decryptedByteCount As Integer = cs.Read(decryptedBytes, 0, decryptedBytes.Length)
            Return Encoding.UTF8.GetString(decryptedBytes, 0, decryptedByteCount)
        End Using
    End Function

    ' If specified key is not a valid size 
    ' 指定されたキーが有効なサイズではない場合
    Function LongKey(key As String, length As Integer) As String
        Dim keyBytes As Byte() = Encoding.UTF8.GetBytes(key)
        If keyBytes.Length >= length Then
            Return key.Substring(0, length)
        End If

        Dim longKeyBytes(length - 1) As Byte
        Array.Copy(keyBytes, longKeyBytes, keyBytes.Length)
        Return Encoding.UTF8.GetString(longKeyBytes)
    End Function

End Module
