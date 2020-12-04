<?php

namespace App\Serializers;

use Flugg\Responder\Contracts\ErrorSerializer as ErrorSerializerContract;

/**
 * Class ErrorSerializer
 */
class ErrorSerializer implements ErrorSerializerContract
{
    /**
     * @inheritDoc
     */
    public function format($message = null, string $errorCode = null, array $data = null): array
    {
        return [
            'message' => $message,
        ];
    }
}
